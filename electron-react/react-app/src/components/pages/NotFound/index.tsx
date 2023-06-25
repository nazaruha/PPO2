import { FC, useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { IStoreProject } from "../Project/types";
import { store } from "../../containers/store";

const NotFoundPage: FC = () => {
    const { isProjectStored, project } = useSelector((store: any) => store.project as IStoreProject);
    const [link, setLink] = useState<string>("");

    useEffect(() => {
        if (project !== undefined) setLink("/main-page")
        else setLink("/");
    }, [])

    return (
        <div className="row mt-3">
            <div className="col-md-12">
                <div className="error-template d-flex aligns-items-center justify-content-center text-center">
                    <div>
                        <h1>
                            Oops!</h1>
                        <h2>
                            404 Not Found</h2>
                        <div className="error-details">
                            Sorry, an error has occured, Requested page not found!
                        </div>
                        <div className="error-actions mt-3">
                            <Link to={`${link}`} className="btn btn-primary btn-lg"><span>Take Me Home</span></Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default NotFoundPage;