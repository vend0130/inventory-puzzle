using UnityEngine;

namespace Code.Infrastructure.Services.Ad
{
    public class EditorAdService : IAdService
    {
        public void Show() => 
            Debug.Log("Showed AD");
    }
}