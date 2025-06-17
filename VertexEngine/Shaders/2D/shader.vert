#version 330 core

layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 TextureCoords;

uniform mat4 transform;
uniform mat4 view;

out vec2 textureCoord;

void main() 
{
    textureCoord = TextureCoords;
    gl_Position = vec4(Position, 1) * transform * view;
}
