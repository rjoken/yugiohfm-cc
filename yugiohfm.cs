using CrowdControl.Common;
using JetBrains.Annotations;

namespace CrowdControl.Games.Packs.YuGiOhForbiddenMemories;

[UsedImplicitly]
public class YuGiOhForbiddenMemories : PS1EffectPack
{
    public YuGiOhForbiddenMemories(UserRecord player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }

    private const uint ADDR_P1LP = 0x0EA004;

    public override EffectList Effects
    {
        get
        {
            List<Effect> effects = new List<Effect> {
                new Effect("Set Player LP to 1", "p1lp1")
            };
            return effects;
        }
    }

    public override ROMTable ROMTable => new List<ROMInfo>(new[] {
        new ROMInfo("Yu-Gi-Oh! Forbidden Memories", null, Patching.Ignore, ROMStatus.ValidPatched, s => true)
    });

    public override Game Game { get; } = new("Yu-Gi-Oh! Forbidden Memories", "YuGiOhForbiddenMemories", "PS1", ConnectorType.PS1Connector);

    protected override bool IsReady(EffectRequest request) => true;

    protected override void StartEffect(EffectRequest request)
    {
        if (!IsReady(request))
        {
            DelayEffect(request, TimeSpan.FromSeconds(2));
            return;
        }

        string[] codeParams = FinalCode(request).Split('_');

        switch (codeParams[0])
        {
            case "p1lp1":
                SetP1LP1(request);
                return;
        }
    }

    private void SetP1LP1(EffectRequest request)
    {
        TryEffect(request, () => Connector.Write16(ADDR_P1LP, 0x0001), () => true, () => { Connector.SendMessage($"{request.DisplayViewer} set your LP to 1"); });
        return;
    }
}