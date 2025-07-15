namespace WackyPayments;

public class PaymentRequest
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    //user info
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PostCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    //payment information
    public string CardNumber { get; set; }
    public string CardName { get; set; }
    public string CardExpiration { get; set; }
    public string CvvCode { get; set; }

}


//test data
//{
//    "id" : "ece7ebfc-8675-402d-a830-851ed3ea5d99",
//    "totalAmount" : 12.99,
//    "firstname" :"test",
//    "lastname" : "test",
//    "email": "email",
//    "address": "1 street",
//    "postcode": "12345",
//    "city" :"city",
//    "country": "country",
//    "cardNumber": "cardnumer",
//    "cardName": "card name",
//    "cardExpiration": "01/28",
//    "cvvCode": "123"

//}