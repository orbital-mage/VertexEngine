#version 330 core

uniform sampler2D image;

in vec2 textureCoord;

out vec4 Output;

void main() {
    Output = texture(image, textureCoord);
}
