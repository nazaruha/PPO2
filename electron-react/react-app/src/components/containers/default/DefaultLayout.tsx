import { Outlet, useNavigate } from "react-router-dom";
import DefaultSideBar from "./DefaultSideBar";
import { useSelector } from "react-redux";
import { IProjectItem, IStoreProject } from "../../pages/Project/types";
import { useEffect } from "react";

const DefaultLayout = () => {
    const { isProjectStored, project } = useSelector((store: any) => store.project as IStoreProject);
    const navigate = useNavigate();

    useEffect(() => {
        if (!isProjectStored) {
            console.log("DefaultLayout.tsx project is stored");
            // navigate("..")
            return;
        }
        console.log("Project is not stored");
    }, [])

    return (
        <>
            <div className="container-fluid">
                <div className="row flex-nowrap">
                    <DefaultSideBar />
                    <div className="col py-3">
                        {isProjectStored && <Outlet />}
                    </div>
                </div>
            </div>
        </>
    )
}

export default DefaultLayout;