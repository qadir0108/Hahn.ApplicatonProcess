using MediatR;
using System.Collections.Generic;

namespace Hahn.ApplicatonProcess.July2021.Domain.Command
{
    public class TrackAssetCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public string AssetId { get; set; }
        public bool TrackUntrack { get; set; }
    }
}