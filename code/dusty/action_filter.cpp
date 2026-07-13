UActionsFilter::UActionsFilter()
{
    PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UActionsFilter::BeginPlay()
{
    Super::BeginPlay();
}


// Called every frame
void UActionsFilter::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
    Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

    if (!IsValid(StatesDataAsset))
    {
        return;
    }

    if (IsValid(m_currentBaseState) && m_currentBaseState->CanUpdateTick)
    {
        m_currentBaseState->Update(DeltaTime);
    }
}

bool UActionsFilter::IsStateAvailable(const TSubclassOf<UBaseState> _state)
{
    if (!IsValid(StatesDataAsset))
    {
        return false;
    }

    for (const auto& Pair : StatesDataAsset->StatePriorityMap)
    {
        if (Pair.Key == m_currentBaseStateClass)
        {
            for (const auto& AvailableState : Pair.Value.AvailableStates)
            {
                if (AvailableState == _state)
                {
                    return true;
                }
            }
        }
    }
    return false;
}

void UActionsFilter::SetCurrentState(TSubclassOf<UBaseState> _newState)
{
    if (!m_statesInstancesMap.Contains(_newState) || _newState == m_currentBaseStateClass)
    {
        return;
    }

    UBaseState* NewState = *m_statesInstancesMap.Find(_newState);

    if (m_currentBaseState != nullptr && NewState != nullptr)
    {
        if (IsStateAvailable(_newState))
        {
            m_currentBaseState->Exit();

            m_currentBaseState = NewState;
            m_currentBaseStateClass = _newState;

            NewState->Init();
        }
    }
}

void UActionsFilter::InitializeFilter(AActor* _owner, TObjectPtr<UStatesDataAsset> _statesDataAsset, const TSubclassOf<UBaseState> _state)
{
    StatesDataAsset = _statesDataAsset;

    if (!IsValid(StatesDataAsset))
    {
        return;
    }

    m_owner = _owner;

    // Pair is of type TSubclassOf<UBaseState>, FAvailableStates>
    for (const auto& Pair : StatesDataAsset->StatePriorityMap)
    {
        if (UBaseState* NewStateInstance = NewObject<UBaseState>(this, Pair.Key))
        {
            m_statesInstancesMap.Add(Pair.Key, NewStateInstance);
            NewStateInstance->SetOwner(m_owner);
        }
        else
        {
            UE_LOG(LogTemp, Error, TEXT("Failed to create instance of state class."));
        }
    }

    m_currentBaseState = *m_statesInstancesMap.Find(_state);
    m_currentBaseStateClass = _state;

    m_currentBaseState->Init();
}