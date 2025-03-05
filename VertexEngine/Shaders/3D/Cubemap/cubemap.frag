#version 330 core

in vec3 textureCoords;

uniform samplerCube cubemap;

out vec4 Output;

void main()
{
    Output = texture(cubemap, textureCoords);
} 
