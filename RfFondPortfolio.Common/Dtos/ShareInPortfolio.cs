namespace RfFondPortfolio.Common.Dtos
{
    public class ShareInPortfolio : PaperInPortfolio<SharePaper>
    {
        public ShareInPortfolio(SharePaper paper)
        {
            Paper = paper;
        }
    }
}