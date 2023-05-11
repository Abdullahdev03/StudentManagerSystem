using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;


[JsonObject(MemberSerialization.OptIn)]
struct oSendMessage
{
    [JsonProperty("status")]
    public string status { get; set; }

    [JsonProperty("timestamp")]
    public DateTime timestamp { get; set; }

    [JsonProperty("txn_id")]
    public string txn_id { get; set; }

    [JsonProperty("msg_id")]
    public string msg_id { get; set; }

    [JsonProperty("smsc_msg_id")]
    public string smsc_msg_id { get; set; }

    [JsonProperty("smsc_msg_status")]
    public string smsc_msg_status { get; set; }

    [JsonProperty("smsc_msg_parts")]
    public string smsc_msg_parts { get; set; }
}

public class SendMessageService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public SendMessageService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


   
    
    
    public async Task<Response<SendMessageDto>> SendMessage(SendMessageDto sms)
    {
        try
        {
            Console.WriteLine("Тестовое приложение OsonSMS!");
            IDictionary<string, string> config = new Dictionary<string, string>();
            config["dlm"] = ";"; // не надо менять!!! 
            config["t"] = "23"; // не надо менять!!!
            config["login"] = "tuttj"; // Ваш логин
            config["pass_hash"] = "52d4c5e2fdbd78d61711a0610d8bba47"; // Ваш хэш код
            config["sender"] = "OsonSMS"; // Ваш алфанумерик

            var phone_number = sms.PhoneNumber;  // = "927747736";
            var msg = "Hello world from C# test";
            var txn_id = "";
            Random random = new Random();
        
            txn_id = random.Next(100000, 100000000).ToString();
            JObject joResponse = SendSMS(config, phone_number, msg, txn_id); // Отправка СМС сообщения

            if (joResponse["error"] != null)
            {
                Console.WriteLine("There is problem in sending message: " + joResponse["error"]["msg"]);
            }
            else
            {
                oSendMessage objArr = JsonConvert.DeserializeObject<oSendMessage>(joResponse.ToString());
                Console.WriteLine("Your message send successful.  Your Id : " + objArr.msg_id);
            
                JObject joResponse2 = CheckSMSStatus(config, objArr.msg_id);
            
                if (joResponse2["error"] != null)
                {
                    Console.WriteLine(" Error to check while  Возникла ошибка при проверки статуса СМС. Причина: " + joResponse2["error"]["msg"]);
                }
                else
                {
                    Console.WriteLine("Status your sms  " + joResponse2["message_state"]);
                }
           
            }
        
            JObject joResponse3 = CheckBalance(config,txn_id); // Проверка баланса
            if (joResponse3["error"] != null)
            {
                Console.WriteLine("Возникла ошибка при проверке баланса. Причина: " + joResponse3["error"]["msg"]);
            }
            else
            {
                Console.WriteLine("Ваш баланс: " + joResponse3["balance"] + "TJS");
            }

            return new Response<SendMessageDto>(sms);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
      
    
    public  JObject SendSMS(IDictionary<string, string> config, string phone_number, string msg, string txn_id)
    {

        try
        {
            //var txn_id = "test_12783"; // Должен быть уникальным для каждого запроса
            var str_hash = Sha256Hash(txn_id + config["dlm"] + config["login"] + config["dlm"] + config["sender"] + config["dlm"] + phone_number + config["dlm"] + config["pass_hash"]);
         
            var client = new RestClient("https://api.osonsms.com/sendsms_v1.php");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddParameter("from", config["sender"]);
            request.AddParameter("login", config["login"]);
            request.AddParameter("t", config["t"]);
            request.AddParameter("phone_number", phone_number);
            request.AddParameter("msg", msg);
            request.AddParameter("str_hash", str_hash);
            request.AddParameter("txn_id", txn_id);
        
            var response = client.Execute(request);
            var content = response.Content; // raw content as string

            JObject joResponse = JObject.Parse(content);
            
            return joResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public  JObject CheckSMSStatus(IDictionary<string, string> config, string msg_id)
        {
            // Параметры, которые могут меняться каждый раз
            var txn_id = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // Должен быть уникальным для каждого запроса
            var str_hash = Sha256Hash(config["login"] + config["dlm"] + txn_id + config["dlm"] + config["pass_hash"]);
        
            var client = new RestClient("http://82.196.1.18/sendsms_v1.php");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddParameter("t", config["t"]);
            request.AddParameter("login", config["login"]);
            request.AddParameter("msg_id", msg_id);
            request.AddParameter("str_hash", str_hash);
            request.AddParameter("txn_id", txn_id);
        
            var response = client.Execute(request);
            var content = response.Content; // raw content as string
            /*
            * Console.WriteLine(content);
            * Ответ сервера при успешной отправки сообщения 
            * {"message_id": "57861909", "final_date": {"date": "2017-12-13 16:44:30.000000", "timezone_type": 1, "timezone": "+05:00"}, "message_state_code": 2, "error_code": 0, "message_state": "Delivered"}
            * При ошибке
            * {"error":{"code":107,"msg":"Message ID is invalid","timestamp":"2017-12-13 16:58:37"}}
            */
            JObject joResponse = JObject.Parse(content);
        
            return joResponse;
        }
        
    public  JObject CheckBalance(IDictionary<string, string> config, string txn_id)
        {
            // Параметры, которые могут меняться каждый раз
            //var txn_id = "test_12342"; // Должен быть уникальным для каждого запроса
            var str_hash = Sha256Hash(txn_id + config["dlm"] + config["login"] + config["dlm"] + config["pass_hash"]);
        
            var client = new RestClient("http://82.196.1.18/check_balance.php");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddParameter("t", config["t"]);
            request.AddParameter("login", config["login"]);
            request.AddParameter("str_hash", str_hash);
            request.AddParameter("txn_id", txn_id);
        
            var response = client.Execute(request);
            var content = response.Content; // raw content as string
            /*
             * Console.WriteLine(content);
             * Ответ сервера {"balance":6.07,"timestamp":"2017-12-13 15:51:00"}
             */
        
            JObject joResponse = JObject.Parse(content);
            return joResponse;
        }
        

    public static String Sha256Hash(String value)
    {
        StringBuilder Sb = new StringBuilder();
        
        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));
        
            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }
        
        return Sb.ToString();
    }
}