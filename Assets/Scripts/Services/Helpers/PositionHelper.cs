using UnityEngine;

namespace Services.Helpers
{
    public static class PositionHelper
    {
        public static float CalculateJumpHeight(float jumpForce, float mass, float gravity = 0f)
        {
            if (gravity <= 0f)
                gravity = Mathf.Abs(Physics.gravity.y);

            if (gravity <= 0f)
                gravity = 9.81f;

            float v0 = jumpForce / mass;
            return (v0 * v0) / (2f * gravity);
        }
    }
}