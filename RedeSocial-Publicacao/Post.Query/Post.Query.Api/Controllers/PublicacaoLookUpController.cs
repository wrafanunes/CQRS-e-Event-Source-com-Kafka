using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Post.Comon.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PublicacaoLookUpController : ControllerBase
    {
        private readonly ILogger<PublicacaoLookUpController> _logger;
        private readonly IQueryDispatcher<PublicacaoEntity> _queryDispatcher;

        public PublicacaoLookUpController(ILogger<PublicacaoLookUpController> logger, IQueryDispatcher<PublicacaoEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> ObterTodasAsPublicacoesAsync()
        {
            try
            {
                var publicacoes = await _queryDispatcher.SendAsync(new BuscarTodasAsPublicacoesQuery());
                return NormalResponse(publicacoes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error ao recuperar todas as publicações";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet]
        [Route("peloId/{id}")]
        public async Task<ActionResult> ObterPorIdAsync(Guid id)
        {
            try
            {
                var publicacao = await _queryDispatcher.SendAsync(new BuscarPublicacaoPorIdQuery { Id = id });

                if ((bool)!publicacao?.Any()) return NoContent();

                var count = publicacao.Count;

                return Ok(new PublicacaoLookUpResponse()
                {
                    Publicacoes = publicacao,
                    Message = $"Publicação retornada com sucesso."
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error ao recuperar todas a publicação pelo id informado";
                _logger.LogError(ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet]
        [Route("peloAutor/{autor}")]
        public async Task<ActionResult> ObterPublicacoesPeloAutorAsync(string autor)
        {
            try
            {
                var publicacoes = await _queryDispatcher.SendAsync(new BuscarPublicacoesPeloAutorQuery { Autor = autor });
                return NormalResponse(publicacoes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error ao recuperar publicações pelo autor";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet]
        [Route("comComentarios")]
        public async Task<ActionResult> ObterPublicacoesComComentariosAsync()
        {
            try
            {
                var publicacoes = await _queryDispatcher.SendAsync(new BuscarPublicacoesComComentariosQuery());
                return NormalResponse(publicacoes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error ao recuperar publicações com comentários";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet]
        [Route("comCurtidas/{numeroCurtidas}")]
        public async Task<ActionResult> ObterPublicacoesComCurtidasAsync(int numeroCurtidas)
        {
            try
            {
                var publicacoes = await _queryDispatcher.SendAsync(new BuscarPublicacoesComCurtidasQuery { NumeroDeCurtidas = numeroCurtidas });
                return NormalResponse(publicacoes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error ao recuperar publicações com curtidas";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<PublicacaoEntity> publicacoes)
        {
            if ((bool)!publicacoes?.Any()) return NoContent();

            var count = publicacoes.Count;

            return Ok(new PublicacaoLookUpResponse()
            {
                Publicacoes = publicacoes,
                Message = $"Successfully returned {count} post{(count > 1 ? 's' : string.Empty)}."
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }
    }
}