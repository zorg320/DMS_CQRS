using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zorg320.DMS.CommandHandler;

namespace Zorg320.DMS.API.Write.DTO
{
    /// <summary>
    /// Classe qui permet de gérer les erreurs 
    /// </summary>
    public class FaultDTO
    {
        /// <summary>
        /// Liste interne des erreurs
        /// </summary>
        private List<ErrorDTO> _errors;
        /// <summary>
        /// Propriété qui affiche la liste des erreurs
        /// </summary>
        public IEnumerable<ErrorDTO> Errors
        {
            get { return _errors; }
            
        }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="errors">liste d'erreur</param>
        public FaultDTO(IEnumerable<Error> errors)
        {
            _errors = new List<ErrorDTO>();
            _errors.AddRange(errors.Select(e => new ErrorDTO(e.Id, e.Message)));
        }


    }
    /// <summary>
    /// Erreur
    /// </summary>
    public class ErrorDTO
    {
        /// <summary>
        /// Identifiant de l'erreir
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// Message de l'erreur
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">identifiant de l'erreuir</param>
        /// <param name="message">Message de l'erreur</param>
        public ErrorDTO(string id, string message)
        {
            Id = id;
            Message = message;
        }

    }
}
