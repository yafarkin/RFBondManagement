namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class Bondization
    {
        public Amortizations amortizations { get; set; }
        public Coupons coupons { get; set; }
        public Offers offers { get; set; }
    }

    public class Amortizations
    {
        public Metadata metadata { get; set; }
        public string[] columns { get; set; }
        public object[][] data { get; set; }
    }

    public class Metadata
    {
        public Isin isin { get; set; }
        public Name name { get; set; }
        public Issuevalue issuevalue { get; set; }
        public Amortdate amortdate { get; set; }
        public Facevalue facevalue { get; set; }
        public Initialfacevalue initialfacevalue { get; set; }
        public Faceunit faceunit { get; set; }
        public Valueprc valueprc { get; set; }
        public Value value { get; set; }
        public Value_Rub value_rub { get; set; }
        public Data_Source data_source { get; set; }
    }

    public class Isin
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Name
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Issuevalue
    {
        public string type { get; set; }
    }

    public class Amortdate
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Facevalue
    {
        public string type { get; set; }
    }

    public class Initialfacevalue
    {
        public string type { get; set; }
    }

    public class Faceunit
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Valueprc
    {
        public string type { get; set; }
    }

    public class Value
    {
        public string type { get; set; }
    }

    public class Value_Rub
    {
        public string type { get; set; }
    }

    public class Data_Source
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Coupons
    {
        public Metadata1 metadata { get; set; }
        public string[] columns { get; set; }
        public object[][] data { get; set; }
    }

    public class Metadata1
    {
        public Isin1 isin { get; set; }
        public Name1 name { get; set; }
        public Issuevalue1 issuevalue { get; set; }
        public Coupondate coupondate { get; set; }
        public Recorddate recorddate { get; set; }
        public Startdate startdate { get; set; }
        public Initialfacevalue1 initialfacevalue { get; set; }
        public Facevalue1 facevalue { get; set; }
        public Faceunit1 faceunit { get; set; }
        public Value1 value { get; set; }
        public Valueprc1 valueprc { get; set; }
        public Value_Rub1 value_rub { get; set; }
    }

    public class Isin1
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Name1
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Issuevalue1
    {
        public string type { get; set; }
    }

    public class Coupondate
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Recorddate
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Startdate
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Initialfacevalue1
    {
        public string type { get; set; }
    }

    public class Facevalue1
    {
        public string type { get; set; }
    }

    public class Faceunit1
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Value1
    {
        public string type { get; set; }
    }

    public class Valueprc1
    {
        public string type { get; set; }
    }

    public class Value_Rub1
    {
        public string type { get; set; }
    }

    public class Offers
    {
        public Metadata2 metadata { get; set; }
        public string[] columns { get; set; }
        public object[] data { get; set; }
    }

    public class Metadata2
    {
        public Isin2 isin { get; set; }
        public Name2 name { get; set; }
        public Issuevalue2 issuevalue { get; set; }
        public Offerdate offerdate { get; set; }
        public Offerdatestart offerdatestart { get; set; }
        public Offerdateend offerdateend { get; set; }
        public Facevalue2 facevalue { get; set; }
        public Faceunit2 faceunit { get; set; }
        public Price price { get; set; }
        public Value2 value { get; set; }
        public Agent agent { get; set; }
        public Offertype offertype { get; set; }
    }

    public class Isin2
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Name2
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Issuevalue2
    {
        public string type { get; set; }
    }

    public class Offerdate
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Offerdatestart
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Offerdateend
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Facevalue2
    {
        public string type { get; set; }
    }

    public class Faceunit2
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Price
    {
        public string type { get; set; }
    }

    public class Value2
    {
        public string type { get; set; }
    }

    public class Agent
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Offertype
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }
}