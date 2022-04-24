using UnityEngine;

namespace MaterialController
{
    public interface IMaterialUpdater
    {
        void SetColor(string name, Color value);
        void SetColor(int nameID, Color value);
        void SetFloat(string name, float value);
        void SetFloat(int nameID, float value);
        

    }

}
