using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.EventStores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zorg320.DMS.Aggregate;
using Zorg320.DMS.API.Write.DTO;
using Zorg320.DMS.Command;

namespace Zorg320.DMS.API.Write.Controllers
{
    /// <summary>
    /// Controlleur d'une catégorie
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        /// <summary>
        /// Bus qui gère les commandes
        /// </summary>
        private ICommandBus _commandBus;

        /// <summary>
        /// Controlleur des catégories (uniquement en écriture)
        /// </summary>
        public CategorieController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
          
        }
        /// <summary>
        /// Création d'une catégorie
        /// </summary>
        /// <returns>Résultat de la création</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategorieId),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FaultDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateCategorieDTO message)
        {
            var categorieId = Aggregate.CategorieId.New;
            //On appel via le commandbus la commande
            var result = await _commandBus.PublishAsync(new CreateCategorieCommand(categorieId, message.Label), default);
            //On vérifie si le résultat est correct 
            if (result.IsSuccess)
            {
                return Ok(categorieId);
            }
            else
            {
                return BadRequest(new FaultDTO(result.Errors));
            }
        }
        /// <summary>
        /// Met à jour le label de la catégorie
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <param name="message">Informations de mise à jour</param>
        /// <returns>Résultat de la mise à jour</returns>
        [HttpPut("{id}/label")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FaultDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLabel(string id, UpdateLabelCategorieDTO message)
        {
            //On appel via le commandbus la commande
            var result = await _commandBus.PublishAsync(new ModifyLabelCategorieCommand(new Aggregate.CategorieId(id), message.NewLabel), default);
            //On vérifie si le résultat est correct 
            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new FaultDTO(result.Errors));
            }
        }
        /// <summary>
        /// Met à jour le label de la catégorie
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <param name="message">Informations de mise à jour</param>
        /// <returns>Résultat de la mise à jour</returns>
        [HttpPut("ReOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FaultDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Reorder(string id, string id2)
        {
            //On appel via le commandbus la commande
            var result = await _commandBus.PublishAsync(new ReOrderCategorieCommand(new Aggregate.CategorieId(id), new Aggregate.CategorieId(id), new Aggregate.CategorieId(id2)), default);
            //On vérifie si le résultat est correct 
            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new FaultDTO(result.Errors));
            }
        }
    }
}