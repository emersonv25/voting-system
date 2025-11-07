using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Application.DTOs;

namespace VotingSystem.Application.Interfaces
{
    public interface IResultService
    {
        Task<ResultDto> GetResultAsync();
    }
}
