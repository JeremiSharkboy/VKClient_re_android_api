namespace VKClient.Common.Backend.DataObjects
{
    public sealed class MoneyTransfersSettings
    {
        //money_p2p_params":{"min_amount":100,
        //"max_amount":75000,
        //"currency":"RUB"},
        public int min_amount { get; set; }

        public int max_amount { get; set; }

        public string currency { get; set; }
    }
}
