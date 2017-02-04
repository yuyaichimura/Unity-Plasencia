using TreeSharpPlus;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

[LibraryIndexAttribute(6)]
public class _SlowCameraMove: SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;

    [Name("Slow Camera Move")]
    public _SlowCameraMove(SmartCamera camera, SmartCameraPosition pos)
        : base(camera, pos)
    {
        this.camera = camera;
        this.pos = pos;
    }

    public override Node BakeTree(Token token)
    {
        return camera.CameraMove(5.0f, pos.transform, 8.0f);
    }
}

[LibraryIndexAttribute(6)]
public class _SlowCameraMove2 : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;


    [Name("Slow Camera Move 2")]
    public _SlowCameraMove2(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2)
        : base(camera, pos, pos2)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(4.0f, pos.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4f, pos2.transform, 4.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowCameraMove2t : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;


    [Name("Slow Camera Move 2t")]
    public _SlowCameraMove2t(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2)
        : base(camera, pos, pos2)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(4.0f, pos.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4f, pos2.transform, 8.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowCameraMove2Interact : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;
    SmartCameraPosition pos3;


    [Name("Slow Camera Move 2 Interact")]
    public _SlowCameraMove2Interact(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2, SmartCameraPosition pos3)
        : base(camera, pos, pos2, pos3)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
        this.pos3 = pos3;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(4.0f, pos.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4f, pos2.transform, 4.0f),
            new LeafWait(4000),
            camera.CameraMove(4f, pos3.transform, 4.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowerCameraMove2Interact : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;
    SmartCameraPosition pos3;


    [Name("Slower Camera Move 2 Interact")]
    public _SlowerCameraMove2Interact(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2, SmartCameraPosition pos3)
        : base(camera, pos, pos2, pos3)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
        this.pos3 = pos3;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(4.0f, pos.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4f, pos2.transform, 6.0f),
            new LeafWait(6000),
            camera.CameraMove(4f, pos3.transform, 4.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowerCameraMove2 : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;


    [Name("Slower Camera Move 2")]
    public _SlowerCameraMove2(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2)
        : base(camera, pos, pos2)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(7.0f, pos.transform, 7.0f),
            new LeafWait(11000),
            camera.CameraMove(7f, pos2.transform, 7.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowestCameraMove2 : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;


    [Name("Slowest Camera Move 2")]
    public _SlowestCameraMove2(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2)
        : base(camera, pos, pos2)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(9.0f, pos.transform, 10.0f),
            new LeafWait(16000),
            camera.CameraMove(9.5f, pos2.transform, 10.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowCameraMove4 : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;
    SmartCameraPosition pos3;
    SmartCameraPosition pos4;


    [Name("Slow Camera Move 4")]
    public _SlowCameraMove4(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2, SmartCameraPosition pos3, SmartCameraPosition pos4)
        : base(camera, pos, pos2, pos3, pos4)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
        this.pos3 = pos3;
        this.pos4 = pos4;

    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(2.5f, pos.transform, 4.0f),
            new LeafWait(10000),
            camera.CameraMove(2.5f, pos2.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(2.5f, pos3.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(2.5f, pos4.transform, 4.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowCameraMove4Interact : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos2;
    SmartCameraPosition pos3;
    SmartCameraPosition pos4;
    SmartCameraPosition pos5;

    [Name("Slow Camera Move 4 Interact")]
    public _SlowCameraMove4Interact(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos2, SmartCameraPosition pos3, SmartCameraPosition pos4, SmartCameraPosition pos5)
        : base(camera, pos, pos2, pos3, pos4, pos5)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos2 = pos2;
        this.pos3 = pos3;
        this.pos4 = pos4;
        this.pos5 = pos5;

    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(2.5f, pos.transform, 4.0f),
            new LeafWait(10000),
            camera.CameraMove(3.5f, pos2.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4.0f, pos3.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4.0f, pos4.transform, 4.0f),
            new LeafWait(4000),
            camera.CameraMove(4.0f, pos5.transform, 4.0f));
    }
}

[LibraryIndexAttribute(6)]
public class _SlowCameraMove5Interact : SmartEvent
{
    SmartCamera camera;
    SmartCameraPosition pos;
    SmartCameraPosition pos1;
    SmartCameraPosition pos2;
    SmartCameraPosition pos3;
    SmartCameraPosition pos4;
    SmartCameraPosition pos5;

    [Name("Slow Camera Move 5 Interact")]
    public _SlowCameraMove5Interact(SmartCamera camera, SmartCameraPosition pos, SmartCameraPosition pos1, SmartCameraPosition pos2, SmartCameraPosition pos3, SmartCameraPosition pos4, SmartCameraPosition pos5)
        : base(camera, pos, pos1, pos2, pos3, pos4, pos5)
    {
        this.camera = camera;
        this.pos = pos;
        this.pos1 = pos1;
        this.pos2 = pos2;
        this.pos3 = pos3;
        this.pos4 = pos4;
        this.pos5 = pos5;

    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(camera.CameraMove(2.5f, pos.transform, 4.0f),
            new LeafWait(1000), 
            camera.CameraMove(2.5f, pos1.transform, 4.0f),
            new LeafWait(10000),
            camera.CameraMove(3.5f, pos2.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4.0f, pos3.transform, 4.0f),
            new LeafWait(8000),
            camera.CameraMove(4.0f, pos4.transform, 4.0f),
            new LeafWait(4000),
            camera.CameraMove(4.0f, pos5.transform, 4.0f));
    }
}


[LibraryIndexAttribute(6)]
public class _CameraFadeBlack: SmartEvent
{
    SmartCamera camera;

    [Name("CameraFade Black")]
    public _CameraFadeBlack(SmartCamera camera)
        : base(camera)
    {
        this.camera = camera;
    }

    public override Node BakeTree(Token token)
    {
        return camera.Node_CameraFadeBlack();
    }
}