#version 330 core

#include ~/_3D/utils.frag

void main()
{
    Output = calcLights() + calcReflection() + calcRefraction();
}