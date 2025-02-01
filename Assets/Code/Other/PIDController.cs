using System;

namespace Other
{
    [Serializable]
    public class PIDController
    {
        public PIDController()
        {
            Kp = 1.0f;
            Ki = 0.1f;
            Kd = 0.05f;
        }
        public PIDController(float kp, float ki, float kd)
        {
            Kp = kp;
            Ki = ki;
            Kd = kd;
        }
        
        
        public float Kp { get; set; } // Proportional
        public float Ki { get; set; } // Integral
        public float Kd { get; set; } // Derivative

        private float m_Integral; // Sum of errors
        private float m_LastError; // Error in the previous update

        
        public float Calculate(float target, float current, float deltaTime)
        {
            float error = target - current;
            m_Integral += error * deltaTime;
            float derivative = (error - m_LastError) / deltaTime;

            m_LastError = error;

            return Kp * error + Ki * m_Integral + Kd * derivative; // Control signal
        }
    }
}