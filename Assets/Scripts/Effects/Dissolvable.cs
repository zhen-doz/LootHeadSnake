using System.Threading.Tasks;
using UnityEngine;

namespace SnakeGame
{
    /**
     * Responsible for Gradual Dissolution Effect
     */
    public class Dissolvable : MonoBehaviour
    {
        private float _secondsToDissolve;
        public float SecondsToDissolve
        {
            get { return _secondsToDissolve; }
            set { _secondsToDissolve = value; }
        }
        
        private static readonly int DissolveState = Shader.PropertyToID("_DissolveState");
        
        // This effect looked mush smoother with Coroutine, but I wanted to try this way - also works
        public async Task StartDissolve()
        {
            Material dissolvingMaterial = GetComponent<Renderer>().material;;
            float dissolveState = 0;
            var endTime = Time.time + SecondsToDissolve;
            var lastTime = Time.time;
            var elapsedTime = 0f;
            while (Time.time < endTime)
            {
                elapsedTime += Time.time - lastTime;
                lastTime = Time.time;
                dissolveState = Mathf.Lerp(0, 1, elapsedTime/ SecondsToDissolve);
                dissolvingMaterial.SetFloat(DissolveState, dissolveState);
                await Task.Yield();
            }

        }
        
        
        
        
    }
}