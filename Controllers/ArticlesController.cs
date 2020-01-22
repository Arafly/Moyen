using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moyen.Features.Articles;

namespace Moyen.Controllers
{
    [Route("articles]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ArticlesEnvelope> Get([FromQuery] string tag, [FromQuery] string author, [FromQuery] string faved) 
        {
            return await _mediator.Send(new List.Query(tag, author, faved ));
        }

        [HttpGet("{slug}")]
        public async Task<ArticleEnvelope> Get(string slug){
            return await _mediator.Send(new Details.Query(slug));
        }

        [HttpPost]
        // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> Create([FromBody]Create.Command command){
            return await _mediator.Send(command);
        }

        [HttpPut("{slug}")]
        // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> Edit(string slug, [FromBody]Edit.Command command){
            // This will be passed across inside the Command object that we're sending by the _mediator
            command.Slug = slug;
            return await _mediator.Send(command);
        }

        [HttpDelete("{slug}")]
        // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete(string slug){
            await _mediator.Send(new Delete.Command(slug));
        }

    }
}