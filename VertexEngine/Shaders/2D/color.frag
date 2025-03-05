#version 330 core

uniform vec3 color;

out vec4 Output;

void main() {
    Output = vec4(color, 1);
}
