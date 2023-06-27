using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Post.Cmd.Api.Commands;
using Post.Comon.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CurtirPublicacaoController : ControllerBase
    {
        private readonly ILogger<CurtirPublicacaoController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public CurtirPublicacaoController(ILogger<CurtirPublicacaoController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> CurtirPublicacaoAsync(Guid id)
        {
            try
            {
                await _commandDispatcher.SendAsync(new CurtirPublicacaoCommand { Id = id });

                return Ok(new BaseResponse
                {
                    Message = "Like post request completed successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Não foi possível recuperar o agregado, o cliente passou um id da publicação incorreto.");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Erro ao curtir a publicação";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}