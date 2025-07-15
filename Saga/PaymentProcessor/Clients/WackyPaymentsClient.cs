using PaymentProcessor.Clients.ExternalTypes;

namespace PaymentProcessor.Clients
{


    public class WackyPaymentsClient : IWackyPaymentsClient
    {
        private readonly HttpClient _httpClient;

        public WackyPaymentsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://swapi.dev/api/");//localhost something
        }

        public async ValueTask<PaymentReponse> CapturePayments(PaymentRequest request)
        {
            // call the client to post something and 
            var response = await _httpClient.PostAsJsonAsync("api/payment/capture", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PaymentReponse>();
            }
            else
            {
                // Handle error response
                //var errorResponse = new PaymentReponse
                //{
                //    Success = false,
                //    Message = $"Error: {response.StatusCode} - {response.ReasonPhrase}"
                //};
                //return errorResponse;
            }
            ///see what it there to do
            return await _httpClient.GetFromJsonAsync<PaymentReponse>($"people/");
        }



    }
}
