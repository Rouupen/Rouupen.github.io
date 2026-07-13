UCLASS(ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class DUSTY_API UActionsFilter : public UActorComponent
{
    GENERATED_BODY()

public:
    UActionsFilter();

    virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

    const UBaseState* GetCurrentBaseState() { return m_currentBaseState; };

    const TSubclassOf<UBaseState> GetCurrentBaseStateClass() { return m_currentBaseStateClass; };

    bool IsStateAvailable(TSubclassOf<UBaseState> _state);

    void SetCurrentState(const TSubclassOf<UBaseState> _newState);

    void InitializeFilter(AActor* _owner, TObjectPtr<UStatesDataAsset> _statesDataAsset, const TSubclassOf<UBaseState> _state);

    UPROPERTY(EditAnywhere)
    TObjectPtr<UStatesDataAsset> StatesDataAsset = nullptr;

protected:
    virtual void BeginPlay() override;

    UPROPERTY()
    TMap<TSubclassOf<UBaseState>, TObjectPtr<UBaseState>> m_statesInstancesMap;

private:
    UPROPERTY(Transient)
    TObjectPtr<AActor> m_owner = nullptr;

    UPROPERTY(Transient)
    TObjectPtr<UBaseState> m_currentBaseState = nullptr;

    UPROPERTY()
    TSubclassOf<UBaseState> m_currentBaseStateClass;
};