using System;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public static class StockPaperConverter
    {
        public static BaseSharePaper Map(JsonPaperDefinition paper)
        {
            var result = new BaseSharePaper
            {
                Id = Guid.NewGuid()
            };

            result.SecId = paper.Description.GetDataForString("name", "SECID", "value");
            result.Name = paper.Description.GetDataForString("name", "NAME", "value");
            result.ShortName = paper.Description.GetDataForString("name", "SHORTNAME", "value");
            result.Isin = paper.Description.GetDataForString("name", "ISIN", "value");
            result.FaceValue = paper.Description.GetDataForDecimal("name", "FACEVALUE", "value");
            result.IssueDate = paper.Description.GetDataForDateTime("name", "ISSUEDATE", "value");
            result.TypeName = paper.Description.GetDataForString("name", "TYPENAME", "value");
            result.Group = paper.Description.GetDataForString("name", "GROUP", "value");
            result.Type = paper.Description.GetDataForString("name", "TYPE", "value");

            return result;
        }
    }
}