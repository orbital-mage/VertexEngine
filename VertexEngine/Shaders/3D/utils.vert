layout(location = 0 ) in vec3 Position;
layout(location = 1 ) in vec2 TextureCoords;
layout(location = 2 ) in vec3 Normal;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

out vec2 textureCoords;
out vec3 normal;
out vec3 fragPos;

vec4 calcPosition();
vec3 calcNormal();
vec3 calcFragPos();

vec4 calcPosition()
{
    return vec4(Position, 1.0) * transform * view * projection;
}

vec3 calcNormal()
{
    return Normal * mat3(transpose(inverse(transform)));
}

vec3 calcFragPos()
{
    return vec3(vec4(Position, 1) * transform);
}