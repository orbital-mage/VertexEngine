#version 330 core

layout(location = 0) in vec3 Position;

uniform mat4 view;
uniform mat4 projection;

out vec3 textureCoords;

void main()
{
    vec4 pos = vec4(Position, 1.0) * mat4(mat3(view)) * projection;
    gl_Position = pos.xyww;
    textureCoords = Position;
}