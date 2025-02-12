using Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPlayerService
    {
        IEnumerable<IActionResult> GetAll();

        IEnumerable<IActionResult> Getplayers(int index, int count);


    }
}
