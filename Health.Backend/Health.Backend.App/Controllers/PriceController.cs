using Health.Backend.Domain.Models.Requests;
using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health.Backend.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly IPrecoService _precoService;
        private readonly ICoberturaRepository _coberturaRepository;

        public PriceController(IPrecoService precoService, ICoberturaRepository coberturaRepository)
        {
            _precoService = precoService;
            _coberturaRepository = coberturaRepository;
        }

        [HttpPost]
        public IActionResult Post(SeguradoModel segurado)
        {
            var preco = _precoService.ObterPrecoParaSegurado(segurado);
            return Ok(preco);
        }
    }
}