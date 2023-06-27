using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Comon.DTOs;

namespace Post.Cmd.Api.Controllers
{
    //o atributo ApiController marca o controlador como um controlador RESTful
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NovaPublicacaoController : ControllerBase
    {
        private readonly ILogger<NovaPublicacaoController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NovaPublicacaoController(ILogger<NovaPublicacaoController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NovaPublicacaoAsync(NovaPublicacaoCommand command)
        {
            Guid id = Guid.NewGuid();
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NovaPublicacaoResponse
                {
                    Id = id,
                    Message = "Publicação criada com sucesso."
                });
            }
            //A InvalidOperationException é retornada quando o cliente falha em enviar os detalhes corretor na requisição
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Erro ao criar uma nova publicação";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NovaPublicacaoResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}