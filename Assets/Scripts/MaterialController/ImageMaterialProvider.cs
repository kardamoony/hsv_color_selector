using UnityEngine;
using UnityEngine.UI;

namespace MaterialController
{
    public class ImageMaterialProvider : MonoBehaviour, IMaterialProvider
    {
        [SerializeField] private Image _image;

        public Material Material => _image.material;

        private void Reset()
        {
            _image = GetComponent<Image>();
        }
    }
}
