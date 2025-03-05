#version 330 core

uniform sampler2D atlas;
uniform vec3 color;

in vec2 textureCoord;

out vec4 Output;

void main()
{
    vec4 textureColor = texture(atlas, textureCoord);
    
    if (textureColor.a == 0) {
        discard;
    }
    
    Output = vec4(color, 1) * textureColor;
}