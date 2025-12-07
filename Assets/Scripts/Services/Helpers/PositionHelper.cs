namespace Services.Helpers
{
    public static class PositionHelper
    {
        public static float CalculateJumpHeight(float jumpForce, float mass, float gravity = 10f)
        {
            float v0 = jumpForce / mass;
            return (v0 * v0) / (2 * gravity);
        }
    }
}