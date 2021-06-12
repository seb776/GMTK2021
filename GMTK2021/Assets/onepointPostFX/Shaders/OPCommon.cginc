#define sat(a) clamp(a, 0.0,1.0)
float2x2 r2d(float a) { float cosa = cos(a); float sina = sin(a); return float2x2(cosa, -sina, sina, cosa); }
