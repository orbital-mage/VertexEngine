#define PI 3.14159265358979323846

vec2 hash(vec2 uv)
{
    uv = vec2( dot(uv, vec2(127.1, 311.7)), dot(uv, vec2(269.5, 183.3)) );
    return -1.0 + 2.0 * fract(sin(uv) * 43758.5453123);
}

float noise(vec2 uv)
{
    const float K1 = 0.366025404; // (sqrt(3)-1)/2;
    const float K2 = 0.211324865; // (3-sqrt(3))/6;

    vec2  i = floor( uv + (uv.x + uv.y) * K1 );
    vec2  a = uv - i + (i.x + i.y) * K2;
    float m = step(a.y, a.x);
    vec2  o = vec2(m, 1.0 - m);
    vec2  b = a - o + K2;
    vec2  c = a - 1.0 + 2.0 * K2;
    vec3  h = max( 0.5 - vec3(dot(a, a), dot(b, b), dot(c, c) ), 0.0 );
    vec3  n = h*h*h*h*vec3( dot(a, hash(i + 0.0)), dot(b, hash(i + o)), dot(c, hash(i + 1.0)));
    return dot( n, vec3(70.0) );
}

float perlin(vec2 uv)
{
    mat2 mat = mat2( 1.6, 1.2, -1.2, 1.6);
    float value;
    value  = 0.5000 * noise( uv ); uv = mat * uv;
    value += 0.2500 * noise( uv ); uv = mat * uv;
    value += 0.1250 * noise( uv ); uv = mat * uv;
    value += 0.0625 * noise( uv ); uv = mat * uv;
    
    return value / 2 + 0.5;
}
