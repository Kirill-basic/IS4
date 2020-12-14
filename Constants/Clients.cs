using System;

namespace Constants
{
    public static class Clients
    {
        public static string ApiOne { get; set; } = "client_id";
        public static string Mvc { get; set; } = "client_id_mvc";

    }


    public static class Secrets
    {
        public static string ApiOneSecret { get; set; } = "client_secret";        
        public static string MvcSecret { get; set; } = "client_secret_mvc";
    }

    public static class Scopes
    {
        public static string ApiOneScope { get; set; } = "ApiOne";
        public static string ApiTwoScope { get; set; } = "ApiTwo";
        public static string MvcScope { get; set; } = "Mvc";
        public static string ApiThreeScope { get; set; } = "ApiThree";
    }
}
