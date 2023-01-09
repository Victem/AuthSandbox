using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipment.Api.Models;

namespace Shipment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentController : ControllerBase
    {
        [HttpPost]
        public ShipmentDto Create(ShipmentDto shipment)
        {
            return shipment;
        }

        [HttpGet("{id}")]
        public ShipmentDto Get(int id)
        {
            switch (id)
            {
                case 1:
                case 2: return new ShipmentDto { Id = 1 };
            }
            return new ShipmentDto {Id = id };
        }

        [HttpDelete("{id}")]
        public void Delete(int id) 
        {
        }

        [HttpPut]
        public void Update(ShipmentDto shipment)
        { }

        [HttpGet("~/")]
        public ShipmentDto Wtf(int id)
        {
            var shipments = new List<ShipmentDto>();
            return new ShipmentDto { Id = id };
        }
    }
}
