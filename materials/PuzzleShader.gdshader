shader_type spatial;

uniform sampler2D albedo_texture;
uniform vec2 uv_offset = vec2(0.0);
uniform vec2 uv_scale = vec2(1.0);
uniform float uv_rotation = 0.0; // In radians

void fragment() {
    vec2 uv = UV;

    // Centered rotation
    uv -= vec2(0.5);
    float s = sin(uv_rotation);
    float c = cos(uv_rotation);
    uv = mat2(vec2(c, -s), vec2(s, c)) * uv;
    uv += vec2(0.5);

    // Apply offset and scale
    uv = uv * uv_scale + uv_offset;

    vec4 albedo = texture(albedo_texture, uv);
    ALBEDO = albedo.rgb;
}
