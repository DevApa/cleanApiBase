using bg.crm.integration.application.dtos.models;
using bg.crm.integration.application.dtos.models.productos.creditos;
using bg.crm.integration.application.dtos.responses;
using bg.crm.integration.application.interfaces.services;
using Microsoft.AspNetCore.Mvc;

namespace bg.crm.integration.api.controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("Productos")]
    [Route("crm-[controller]/v1")]
    public class ProductosControllers : BaseApiController
    {
        private readonly IProductoService _productoService;
        public ProductosControllers(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Route("productos/creditos-resumen")]
        [ProducesResponseType(typeof(MsDtoResponseSuccess<ServiceResponseDto<CreditoResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCreditosResumen([FromQuery] CreditoRequestDto request)
        {
            var response = await _productoService.ConsultarResumenCreditoServiceAsync(request);
            return Ok(new MsDtoResponseSuccess<ServiceResponseDto<CreditoResponseDto>>(HttpContext.TraceIdentifier, response));
        }
    }
}