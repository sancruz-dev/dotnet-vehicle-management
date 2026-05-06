using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.ModelViews;

namespace minimal_api.Domain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class VeiculosController : Controller
    {


        private readonly IVeiculoService _veiculoService;
        ErrosValidacao validaDTO(VeiculoDTO veiculoDTO)
        {

            var validacao = new ErrosValidacao();

            if (string.IsNullOrEmpty(veiculoDTO.Nome))
                validacao.Mensagens.Add("O nome não pode ser vazio");

            if (string.IsNullOrEmpty(veiculoDTO.Marca))
                validacao.Mensagens.Add("A marca não pode ficar em branco");

            if (veiculoDTO.Ano < 1950)
                validacao.Mensagens.Add("Veículo muito antigo, são válidos apenas anos superiores.");

            return validacao;
        }

        public VeiculosController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }


        [HttpPost("veiculo")]
        public IActionResult CadastraVeiculo([FromBody] VeiculoDTO veiculoDTO)
        {
            if (validaDTO(veiculoDTO).Mensagens.Count() > 0)
                return BadRequest(validaDTO(veiculoDTO));

            var veiculo = new Veiculo
            {
                Nome = veiculoDTO.Nome,
                Marca = veiculoDTO.Marca,
                Ano = veiculoDTO.Ano
            };
            _veiculoService.Incluir(veiculo);

            return Created($"/veiculo/{veiculo.Id}", veiculo);
        }

        [HttpPut("veiculo/{id}")]
        [Authorize(Roles = "Adm")]
        public IActionResult AtualizaVeiculo([FromRoute] int id, VeiculoDTO veiculoDTO)
        {
            var veiculo = _veiculoService.BuscaPorId(id);

            if (veiculo == null) return NotFound();

            if (validaDTO(veiculoDTO).Mensagens.Count() > 0)
                return BadRequest(validaDTO(veiculoDTO));

            veiculo.Nome = veiculoDTO.Nome;
            veiculo.Marca = veiculoDTO.Marca;
            veiculo.Ano = veiculoDTO.Ano;
            _veiculoService.Atualizar(veiculo);

            return Ok(veiculo);
        }

        [HttpGet("veiculos")]
        public IActionResult RecuperaVeiculos([FromQuery] int? pagina)
        {
            var veiculos = _veiculoService.Todos(pagina);
            return Ok(veiculos);
        }

        [HttpGet("veiculo/{id}")]
        public IActionResult RecuperaVeiculo([FromRoute] int id)
        {
            var veiculo = _veiculoService.BuscaPorId(id);

            if (veiculo == null) return NotFound();

            return Ok(veiculo);
        }

        [HttpDelete("veiculo/{id}")]
        [Authorize(Roles = "Adm")]
        public IActionResult AtualizaVeiculo([FromRoute] int id)
        {
            var veiculo = _veiculoService.BuscaPorId(id);

            if (veiculo == null) return NotFound();

            _veiculoService.Apagar(veiculo);

            return NoContent();
        }
    }
}