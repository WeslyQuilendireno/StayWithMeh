-- Custom Access Token Hook: injects the user's hotel_role and branch_id
-- from user_profiles into the JWT's claims so ASP.NET Core can read them
-- via [Authorize(Roles = "...")] without an extra database call per request.

CREATE OR REPLACE FUNCTION public.custom_access_token_hook(event jsonb)
RETURNS jsonb
LANGUAGE plpgsql
STABLE
AS $$
DECLARE
    claims jsonb;
    user_role hotel_role;
    user_branch uuid;
BEGIN
    SELECT role, branch_id INTO user_role, user_branch
    FROM public.user_profiles
    WHERE id = (event->>'user_id')::uuid;

    claims := event->'claims';

    IF user_role IS NOT NULL THEN
        claims := jsonb_set(claims, '{role}', to_jsonb(user_role::text));
    ELSE
        claims := jsonb_set(claims, '{role}', to_jsonb('Guest'::text));
    END IF;

    IF user_branch IS NOT NULL THEN
        claims := jsonb_set(claims, '{branch_id}', to_jsonb(user_branch::text));
    END IF;

    event := jsonb_set(event, '{claims}', claims);
    RETURN event;
END;
$$;

-- Allow the Supabase Auth service to call this function
GRANT EXECUTE ON FUNCTION public.custom_access_token_hook TO supabase_auth_admin;

-- Allow the function to read user_profiles (runs as definer-neutral, needs explicit grant)
GRANT SELECT ON public.user_profiles TO supabase_auth_admin;
