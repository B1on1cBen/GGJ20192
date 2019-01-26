/// © Benjamin Gordon, 2017
/// Written with reference from https://gist.github.com/ftvs/5822103 (link live as of 1.17.2017)
/// 
/// Simply attach this script to any object you want to have shake,
/// Then, call the Shake() function, pass it a duration and amount of shake, and Voilà!
/// 
/// This class was originally intended for camera shake, but it can also be used for
/// any object that needs to shake!



/// BUG WARNING: If Shake() appears to have no effect, check if the object you want to 
/// shake has LateUpdate() (usually a camera). If so, you will need to modify Unity's 
/// script execution order, putting your object's script first, and Shaker second.
/// Go here if you're lost https://docs.unity3d.com/Manual/class-ScriptExecution.html (link live as of 1.17.2017)

using UnityEngine;

namespace Medley
{
    public enum Axis
    {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ
    }
}

namespace Medley.Effects
{
    public class Shaker : MonoBehaviour
    {
        Axis shakeAxis = Axis.XY;
        public float shakeAmount;
        public float smoothing;
        bool shake = true;
        Vector3 previousRandomVector = Vector3.zero;
        Vector3 wantedPositon;

        void Awake()
        {
            wantedPositon = transform.localPosition;
        }

        void LateUpdate()
        {
            if (shake)
            {
                Vector3 randomVector = Vector3.zero;

                randomVector = Random.insideUnitSphere * shakeAmount;
                randomVector = GetClampedShakeVector(randomVector);

                wantedPositon += -previousRandomVector + randomVector;
                previousRandomVector = randomVector;

                transform.localPosition = Vector3.Lerp(transform.localPosition, wantedPositon, Time.deltaTime * smoothing);
            }
        }

        public void ShakeOnce(float duration, float amount, Axis shakeAxis = Axis.XY)
        {
            shake = true;
            shakeAmount = amount;
            this.shakeAxis = shakeAxis;
            Invoke("TurnOffShake", duration);
        }

        public void ShakeContinuous(float amount)
        {
            shake = true;
            shakeAmount = amount;
            shakeAxis = Axis.XY;
        }

        public void ShakeContinuous(float amount, Axis shakeAxis)
        {
            shake = true;
            shakeAmount = amount;
            this.shakeAxis = shakeAxis;
        }

        public void ShakeContinuousX(float amount)
        {
            shake = true;
            shakeAmount = amount;
            shakeAxis = Axis.X;
        }

        public void ShakeContinuousY(float amount)
        {
            shake = true;
            shakeAmount = amount;
            shakeAxis = Axis.Y;
        }

        public void TurnOffShake()
        {
            if (shake == true)
            {
                shake = false;
                previousRandomVector = Vector3.zero;
                shakeAxis = Axis.XY;
            }
        }

        Vector3 GetClampedShakeVector(Vector3 originalVector)
        {
            switch (shakeAxis)
            {
                case Axis.XYZ:
                    return originalVector;

                case Axis.X:
                    originalVector.y = 0;
                    originalVector.z = 0;
                    return originalVector;

                case Axis.Y:
                    originalVector.x = 0;
                    originalVector.z = 0;
                    return originalVector;

                case Axis.Z:
                    originalVector.x = 0;
                    originalVector.y = 0;
                    return originalVector;

                case Axis.XY:
                    originalVector.z = 0;
                    return originalVector;

                case Axis.XZ:
                    originalVector.y = 0;
                    return originalVector;

                case Axis.YZ:
                    originalVector.x = 0;
                    return originalVector;
            }

            return originalVector;
        }
    }
}