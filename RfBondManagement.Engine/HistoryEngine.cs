﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine
{
    public class HistoryEngine
    {
        protected readonly IHistoryRepository _historyRepository;
        protected readonly IExternalImport _import;
        protected readonly ILogger _logger;

        public HistoryEngine(IHistoryRepository historyRepository, IExternalImportFactory importFactory, ILogger logger)
        {
            _historyRepository = historyRepository;
            _import = importFactory.GetDefaultImpl();
            _logger = logger;
        }

        public DateTime? GetLastHistoryDate(string secId)
        {
            var when = _historyRepository.Get()
                .Where(s => s.SecId == secId)
                .OrderByDescending(s => s.When)
                .FirstOrDefault()?.When;
            return when;
        }

        public IEnumerable<HistoryPrice> GetHistoryPrices(string secId, DateTime? from = null, DateTime? to = null)
        {
            var result = _historyRepository.Get().Where(s => s.SecId == secId);
            if (from.HasValue)
            {
                result = result.Where(s => s.When >= from);
            }

            if (to.HasValue)
            {
                result = result.Where(s => s.When <= to);
            }

            return result;
        }

        public async Task ImportHistory(string secId)
        {
            var when = GetLastHistoryDate(secId);
            var paper = await _import.ImportPaper(_logger, secId);

            if (null == when || paper.IssueDate > when)
            {
                when = paper.IssueDate;
            }

            var history = await _import.HistoryPrice(_logger, paper, when);
            if (null == history)
            {
                return;
            }

            foreach (var price in history)
            {
                _historyRepository.Insert(price);
            }

            when = GetLastHistoryDate(secId);
        }
    }
}