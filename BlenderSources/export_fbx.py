# RotaryMeter.blend -> Assets/MeterAssets_madebyRadianN/RotaryMeter/Models/*.fbx
# Textures/RotaryMeter_{0,1}.png -> Assets/MeterAssets_madebyRadianN/RotaryMeter/Textures/（原本は同フォルダの .psd。書き出した PNG をここに置く）
#
# 使い方:
#   ヘッドレス: "<blender.exe>" --background BlenderSources/RotaryMeter.blend --python BlenderSources/export_fbx.py
#   Blender MCP: execute_blender_code で exec(open(bpy.path.abspath("//export_fbx.py")).read())
#
# 2026-09-26 に Unity 側の既存 FBX（2021 年・Blender 2.83 出力）と頂点数・三角形数・バウンズ・ノード変換・fileID が
# 一致することを確認した設定。変えると RotaryMeter.prefab の参照（メッシュ名 "Box" / "Roter.000"、ノードの scale 100）が壊れる。
# - オブジェクト名 = Unity のメッシュ名。"Roter.000" のまま（リネーム禁止）
# - 1 ファイル 1 オブジェクト。use_selection=True の前に必ず全選択解除（RSC AGENTS 罠 21）
# - スケールは FBX_SCALE_NONE（Unity では node scale 100 × mesh 0.01 になる。prefab 側が lscale 1000 で吸収している）
import bpy, os, shutil

EXPORTS = (("Box", "RotaryMeter_Box.fbx"), ("Roter.000", "RotaryMeter_Roter.fbx"))
OUT_DIR = bpy.path.abspath("//../Assets/MeterAssets_madebyRadianN/RotaryMeter/Models/")

for obj_name, fname in EXPORTS:
    bpy.ops.object.select_all(action='DESELECT')
    obj = bpy.data.objects[obj_name]
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.export_scene.fbx(
        filepath=os.path.join(OUT_DIR, fname), use_selection=True,
        apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE',
        axis_forward='-Z', axis_up='Y', bake_space_transform=False,
        object_types={'MESH'}, use_mesh_modifiers=True, mesh_smooth_type='OFF', use_tspace=False,
        add_leaf_bones=False, bake_anim=False, path_mode='AUTO', embed_textures=False)
    print(f"[export_fbx] {obj_name} -> {fname}")

TEX_SRC = bpy.path.abspath("//Textures/")
TEX_DST = bpy.path.abspath("//../Assets/MeterAssets_madebyRadianN/RotaryMeter/Textures/")
for fname in ("RotaryMeter_0.png", "RotaryMeter_1.png"):
    shutil.copyfile(os.path.join(TEX_SRC, fname), os.path.join(TEX_DST, fname))
    print(f"[export_fbx] texture {fname} -> Assets")
