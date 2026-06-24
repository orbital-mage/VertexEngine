#version 330 core

uniform sampler2D atlas;
uniform vec3 color;

in vec2 textureCoord;

out vec4 Output;

void main()
{
    float alpha = texture(atlas, textureCoord).a;
    Output = vec4(color, alpha);
}