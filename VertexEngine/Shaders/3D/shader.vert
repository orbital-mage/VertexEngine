#version 330 core

#include ~/_3D/utils.vert

void main(void)
{
    gl_Position = calcPosition();
    
    textureCoords = TextureCoords;
    normal = calcNormal();
    fragPos = calcFragPos();
}