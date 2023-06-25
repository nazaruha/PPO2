import { Outlet } from "react-router-dom";

const ProjectChooseLayout = () => {
    return (
        <>
            <div className="container mt-3">
                <Outlet />
            </div>
        </>
    )
}

export default ProjectChooseLayout;
