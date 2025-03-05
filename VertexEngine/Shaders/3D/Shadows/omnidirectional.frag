#version 330 core

uniform vec3 lightPos;
uniform float farPlane;

in vec4 fragPos;

void main()
{
    float lightDistance = length(fragPos.xyz - lightPos);
    lightDistance = lightDistance / farPlane;
    gl_FragDepth = lightDistance;
}  