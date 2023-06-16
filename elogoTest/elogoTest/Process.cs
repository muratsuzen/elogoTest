
namespace elogoTest
{
    public class Process
    {
        public void Test()
        {
            using(ELogoPostBoxService.PostBoxServiceClient svc = new ELogoPostBoxService.PostBoxServiceClient(ELogoPostBoxService.PostBoxServiceClient.EndpointConfiguration.PostBoxServiceEndpoint))
            {
                ELogoPostBoxService.LoginType login = new ELogoPostBoxService.LoginType(); ;
                login.userName = "test";
                login.passWord = "test";
                var request = new ELogoPostBoxService.LoginRequest(login);

                string sessionId;
                var responseLogin = svc.LoginAsync(request).Result;
                if (responseLogin.LoginResult)
                {
                    sessionId = responseLogin.sessionID;

                    string[] paramList = new string[3];
                    paramList[0] = "DOCUMENTTYPE=RECEIPTADVICE";

                    ELogoPostBoxService.ElementType element = new ELogoPostBoxService.ElementType();
                    var docRequest = new ELogoPostBoxService.GetDocumentRequest(sessionId,paramList);                    
                    var responseDoc = svc.GetDocumentAsync(docRequest).Result;

                    if(responseDoc.GetDocumentResult.resultCode == 1)
                    {
                        File.WriteAllBytes(@"c:\x\" + element.fileName, element.binaryData.Value);
                        svc.GetDocumentDoneAsync(sessionId,element.fileName,paramList);
                        Console.WriteLine("Belge alındı " + responseDoc.GetDocumentResult.resultMsg);
                        
                    }
                    else
                    {
                        Console.Write("Gönderilmedi "+ responseDoc.GetDocumentResult.resultMsg);
                    }

                }
            }

        }
    }
}
